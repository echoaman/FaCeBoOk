package com.example.feedservice.services;

import java.time.LocalDateTime;
import java.time.format.DateTimeFormatter;
import java.util.List;
import java.util.concurrent.CompletableFuture;
import java.util.concurrent.Future;

import com.example.feedservice.dataaccess.PostRepository;
import com.example.feedservice.models.Post;
import com.example.feedservice.interfaces.IPostCache;
import com.example.feedservice.interfaces.IPostService;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.core.ParameterizedTypeReference;
import org.springframework.scheduling.annotation.Async;
import org.springframework.scheduling.annotation.AsyncResult;
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
    public Future<Boolean> savePost(Post post) throws Exception{
        post.setPostedOn(getCurrentDateTime());

        // Save to db
        postRepository.savePost(post.getUid(), post.getPostedOn(), post.getContent());
        log.info("savePost - Post inserted to db");

        // CompletableFuture<List<Post>> future = postRepository.findPostsByUid(post.getUid());
        // List<Post> userPosts = future.get();
        
        // cache all posts
        // for(Post userPost : userPosts) {
        //     if(!postCache.savePost(userPost)) {
        //         return false;
        //     }
        // }

        return new AsyncResult<Boolean>(true);
    }

    @Override
    @Async
    public Future<List<Post>> getPostsByUid(String uid) throws Exception{
        List<Post> posts = null;

        // get from cache
        // log.info("getPostsByUid - Checking posts in cache");
        // posts = postCache.getPostsByUid(uid);

        // if (posts != null && !posts.isEmpty()) {
        //     return posts;
        // }

        // get from db
        log.info("getPostsByUid - Getting posts from db");
        CompletableFuture<List<Post>> future = postRepository.findPostsByUid(uid);
        posts = future.get();
        
        // cache posts
        // for (Post post : posts) {
        //     if(!postCache.savePost(post)) {
        //         return null;
        //     }
        // }

        return new AsyncResult<List<Post>>(posts);
    }
    
    @Override
    @Async
    public Future<List<Post>> getAllPosts() throws Exception {
            //get from db
        CompletableFuture<List<Post>> future = postRepository.findAllPosts();
        List<Post> posts = future.get();
        return new AsyncResult<List<Post>>(posts);
    }
    
    @Override
    @Async
    public List<Post> getFeedForUid(String uid) throws Exception {
        // final List<String> friends = new ArrayList<>();

        // getFriends(uid).subscribe(userFriends -> friends.addAll(userFriends));

        // List<Post> feedPosts = getPostsByUid(uid);
        // if(feedPosts == null || feedPosts.size() == 0) {
        //     feedPosts = new ArrayList<>();
        // }

        // for(String friend : friends) {
        //     List<Post> friendPosts = getPostsByUid(friend);
        //     if(friendPosts != null && !friendPosts.isEmpty()) {
        //         feedPosts.addAll(friendPosts);
        //     }
        // }
        
        // return feedPosts;
        return null;
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
        System.out.println("Before formatting: " + myDateObj);
        DateTimeFormatter myFormatObj = DateTimeFormatter.ofPattern("dd-MM-yyyy HH:mm:ss");
    
        String formattedDate = myDateObj.format(myFormatObj);
        return formattedDate;
    }
}
