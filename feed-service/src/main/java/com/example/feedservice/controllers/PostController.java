package com.example.feedservice.controllers;

import java.util.List;
import java.util.concurrent.CompletableFuture;

import com.example.feedservice.interfaces.IPostService;
import com.example.feedservice.models.Post;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RestController;

@RestController
public class PostController {
    
    private final IPostService postService;

    @Autowired
    public PostController(IPostService postService) {
        this.postService = postService;
    }

    @GetMapping(path="/posts")
    public ResponseEntity<?> getAllPosts() throws Exception {
        CompletableFuture<List<Post>> future = postService.getAllPosts();
        List<Post> posts = future.get();
        if(posts == null || posts.isEmpty()) {
            return new ResponseEntity<>(null, HttpStatus.NOT_FOUND);
        }

        return new ResponseEntity<>(posts, HttpStatus.OK);
    } 

    @GetMapping(path = "/posts/user/{uid}")
    public ResponseEntity<?> getPostByUserId(@PathVariable String uid) throws Exception {
        CompletableFuture<List<Post>> future = postService.getPostsByUid(uid);
        List<Post> posts = future.get();
        if(posts == null || posts.isEmpty()) {
            return new ResponseEntity<>(null, HttpStatus.NOT_FOUND);
        }

        return new ResponseEntity<>(posts, HttpStatus.OK); 
    }

    @PostMapping(path = "/posts")
    public ResponseEntity<?> savePost(@RequestBody Post post) throws Exception {
        CompletableFuture<Boolean> future = postService.savePost(post);
        boolean isPostSaved = future.get();
        if(isPostSaved) {
            return new ResponseEntity<>(HttpStatus.CREATED);
        }

        return new ResponseEntity<>(HttpStatus.BAD_REQUEST);
    }

    @GetMapping(path = "/feed/{uid}")
    public ResponseEntity<?> getFeedForUserId(@PathVariable String uid)  throws Exception {
        CompletableFuture<List<Post>> future = postService.getFeedForUid(uid);
        List<Post> posts = future.get();
        if(posts == null || posts.isEmpty()) {
            return new ResponseEntity<>(null, HttpStatus.NOT_FOUND);
        }

        return new ResponseEntity<>(posts, HttpStatus.OK);
    }
}
