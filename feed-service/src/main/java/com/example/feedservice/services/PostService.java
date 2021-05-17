package com.example.feedservice.services;

import java.time.LocalDateTime;
import java.time.format.DateTimeFormatter;
import java.util.ArrayList;
import java.util.List;
import java.util.concurrent.CompletableFuture;

import com.example.feedservice.dataaccess.PostRepository;
import com.example.feedservice.models.Post;
import com.example.feedservice.interfaces.IPostCache;
import com.example.feedservice.interfaces.IPostService;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.core.ParameterizedTypeReference;
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
    public CompletableFuture<Boolean> savePost(Post post) {
        try {
            post.setPostedOn(getCurrentDateTime());
        
            log.info("Inserting post in db");
            CompletableFuture.supplyAsync(() -> {
                return postRepository.save(post);
            });

            log.info("caching the post");
            CompletableFuture<Boolean> cacheFuture = CompletableFuture.supplyAsync(() -> {
                try {
                    return postCache.savePost(post);
                } catch (Exception e) {
                    e.printStackTrace();
                }
                return false;
            });

            return CompletableFuture.completedFuture(cacheFuture.get());
        } catch (Exception e) {
            log.error(e.getMessage());
        }

        return CompletableFuture.completedFuture(false);
    }

    @Override
    @Async
    public CompletableFuture<List<Post>> getPostsByUid(String uid) {
        try {
            log.info("Checking cache for posts");
            CompletableFuture<List<Post>> checkCacheFuture = CompletableFuture.supplyAsync(() -> {
                try {
                    return postCache.getPostsByUid(uid);
                } catch (Exception e) {
                    e.printStackTrace();
                }
                return null;
            });

            List<Post> postsFromCache = checkCacheFuture.get();
            if(postsFromCache != null && !postsFromCache.isEmpty()) {
                return CompletableFuture.completedFuture(postsFromCache);
            }

            log.info("Getting posts from db");
            CompletableFuture<List<Post>> getDbFuture = postRepository.findPostsByUid(uid);
            final List<Post> posts = getDbFuture.get();
            
            log.info("Cache posts");
            CompletableFuture.runAsync(() -> {
                try {
                    postCache.saveAllPosts(posts);
                } catch (Exception e) {
                    e.printStackTrace();
                }
            });
            
            return CompletableFuture.completedFuture(posts);
        } catch (Exception e) {
            log.error(e.getMessage());
        }
        return CompletableFuture.completedFuture(null);
    }
    
    @Override
    @Async
    public CompletableFuture<List<Post>> getAllPosts() {
        try {
            log.info("Getting all posts");
            CompletableFuture<List<Post>> future = postRepository.findAllPosts();
            List<Post> posts = future.get();
            return CompletableFuture.completedFuture(posts);
        } catch (Exception e) {
            log.error(e.getMessage());
        }
        return CompletableFuture.completedFuture(null);
    }
    
    @Override
    @Async
    public CompletableFuture<List<Post>> getFeedForUid(String uid) {
        try {
            final List<String> friends = new ArrayList<>();

            getFriends(uid).subscribe(userFriends -> friends.addAll(userFriends));

            List<Post> feedPosts = getPostsByUid(uid).get();
            if(feedPosts == null || feedPosts.size() == 0) {
                feedPosts = new ArrayList<>();
            }

            for(String friend : friends) {
                List<Post> friendPosts = getPostsByUid(friend).get();
                if(friendPosts != null && !friendPosts.isEmpty()) {
                    feedPosts.addAll(friendPosts);
                }
            }
            
            return CompletableFuture.completedFuture(feedPosts);
        } catch (Exception e) {
            log.error(e.getMessage());
        }
        return CompletableFuture.completedFuture(null);
    }

    private Mono<List<String>> getFriends(String uid) {
        String uri = "http://localhost:5000/friends/" + uid;

        return wBuilder
            .build()
            .get()
            .uri(uri)
            .retrieve()
            .bodyToMono(new ParameterizedTypeReference<List<String>>() {});
    }

    private String getCurrentDateTime() {
        LocalDateTime myDateObj = LocalDateTime.now();
        DateTimeFormatter myFormatObj = DateTimeFormatter.ofPattern("dd-MM-yyyy HH:mm:ss");
        String formattedDate = myDateObj.format(myFormatObj);
        return formattedDate;
    }
}
