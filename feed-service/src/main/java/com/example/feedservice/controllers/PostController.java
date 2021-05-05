package com.example.feedservice.controllers;

import java.util.List;

import com.example.feedservice.interfaces.IPostService;
import com.example.feedservice.models.Post;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.scheduling.annotation.Async;
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
    @Async
    public ResponseEntity<?> getAllPosts() {
        List<Post> posts = postService.getAllPosts();
        if(posts == null || posts.isEmpty()) {
            return new ResponseEntity<>(null, HttpStatus.NOT_FOUND);
        }

        return new ResponseEntity<>(posts, HttpStatus.OK);
    } 

    @GetMapping(path = "/posts/user/{uid}")
    @Async
    public ResponseEntity<?> getPostByUserId(@PathVariable String uid) {
        List<Post> posts = postService.getPostsByUid(uid);
        if(posts == null || posts.isEmpty()) {
            return new ResponseEntity<>(null, HttpStatus.NOT_FOUND);
        }

        return new ResponseEntity<>(posts, HttpStatus.OK); 
    }

    @PostMapping(path = "/posts")
    @Async
    public ResponseEntity<?> savePost(@RequestBody Post post) {
        boolean isPostSaved = postService.savePost(post);
        if(isPostSaved) {
            return new ResponseEntity<>(HttpStatus.CREATED);
        }

        return new ResponseEntity<>(HttpStatus.BAD_REQUEST);
    }

    @GetMapping(path = "/feed/{uid}")
    @Async
    public ResponseEntity<?> getFeedForUserId(@PathVariable String uid) {
        List<Post> posts = postService.getFeedForUid(uid);
        if(posts == null || posts.isEmpty()) {
            return new ResponseEntity<>(null, HttpStatus.NOT_FOUND);
        }

        return new ResponseEntity<>(posts, HttpStatus.OK);
    }
}