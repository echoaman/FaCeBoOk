package com.example.feedservice.services;

import java.time.LocalDateTime;
import java.time.format.DateTimeFormatter;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;

import com.example.feedservice.dataaccess.PostRepository;
import com.example.feedservice.models.Post;
import com.example.feedservice.interfaces.IPostCache;
import com.example.feedservice.interfaces.IPostService;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.scheduling.annotation.Async;
import org.springframework.stereotype.Service;
import org.springframework.web.reactive.function.client.WebClient;

import lombok.extern.slf4j.Slf4j;
import reactor.core.publisher.Mono;

@Service
@Slf4j
public class PostService implements IPostService {
    private final PostRepository postRepository;
    private final IPostCache postCache;

    @Autowired
    private WebClient.Builder wBuilder;

    @Autowired
    public PostService(PostRepository postRepository, IPostCache postCache) {
        this.postRepository = postRepository;
        this.postCache = postCache;
    }

    @Override
    @Async
    public boolean savePost(Post post) {
        try {
            post.setPostedOn(getCurrentDateTime());

            // Save to db
            postRepository.save(post);
            log.info("savePost - Post inserted to db");

            List<Post> userPosts = postRepository.findPostsByUid(post.getUid());
            
            // cache all posts
            for(Post userPost : userPosts) {
                if(!postCache.savePost(userPost)) {
                    return false;
                }
            }

            return true;
        } catch (Exception ex) {
            log.error("savePost - " + ex.toString());
            throw ex;
        }
    }

    @Override
    @Async
    public List<Post> getPostsByUid(String uid) {
        try {
            List<Post> posts = null;

            // get from cache
            log.info("getPostsByUid - Checking posts in cache");
            posts = postCache.getPostsByUid(uid);

            if (posts != null && !posts.isEmpty()) {
                return posts;
            }

            // get from db
            log.info("getPostsByUid - Getting posts from db");
            posts = postRepository.findPostsByUid(uid);

            // cache posts
            for (Post post : posts) {
                if(!postCache.savePost(post)) {
                    return null;
                }
            }

            return posts;
        } catch (Exception ex) {
            log.error("getPostsByUid - " + ex.toString());
            throw ex;
        }
    }
    
    @Override
    @Async
    public List<Post> getAllPosts() {
        try {
            //get from db
            List<Post> posts = postRepository.findAll();
            return posts;
        } catch (Exception ex) {
            log.error("getAllPosts - "+ ex.getMessage());
            throw ex;
        }
    }
    
    @Override
    @Async
    public List<Post> getFeedForUid(String uid) {
        try {
            final List<String> friends = new ArrayList<>();

            getFriends(uid).subscribe(userFriends -> friends.addAll(Arrays.asList(userFriends)));

            List<Post> feedPosts = getPostsByUid(uid);
            if(feedPosts == null || feedPosts.size() == 0) {
                feedPosts = new ArrayList<>();
            }

            for(String friend : friends) {
                List<Post> friendPosts = getPostsByUid(friend);
                if(friendPosts != null && !friendPosts.isEmpty()) {
                    feedPosts.addAll(friendPosts);
                }
            }
            
            return feedPosts;

        } catch (Exception ex) {
            log.error("getFeedForUid - "+ ex.getMessage());
            throw ex;
        }
    }

    private Mono<String[]> getFriends(String uid) {
        String uri = "http://localhost:5000/friends/" + uid;

        return wBuilder
            .build()
            .get()
            .uri(uri)
            .retrieve()
            .bodyToMono(String[].class);
    }

    private String getCurrentDateTime() {
        LocalDateTime myDateObj = LocalDateTime.now();
        System.out.println("Before formatting: " + myDateObj);
        DateTimeFormatter myFormatObj = DateTimeFormatter.ofPattern("dd-MM-yyyy HH:mm:ss");
    
        String formattedDate = myDateObj.format(myFormatObj);
        return formattedDate;
    }
}
