package com.example.feedservice.controllers;

import java.util.List;

import com.example.feedservice.models.Post;
import com.example.feedservice.services.PostService;

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
    
    private final PostService postService;

    @Autowired
    public PostController(PostService postService) {
        this.postService = postService;
    }

    @GetMapping(path = "/posts")
    public ResponseEntity<?> getAllPosts() {
        List<Post> posts = postService.getAllPosts();
        if(posts == null || posts.isEmpty()) {
            return new ResponseEntity<>(null, HttpStatus.NOT_FOUND);
        }

        return new ResponseEntity<>(posts, HttpStatus.OK); 
    }

    @GetMapping(path = "/posts/{postid}")
    public ResponseEntity<?> getPost(@PathVariable int postid) {
        Post post = postService.getPost(postid);
        if(post == null) {
            return new ResponseEntity<>(null, HttpStatus.NOT_FOUND);
        }

        return new ResponseEntity<>(post, HttpStatus.OK);
    }

    @GetMapping(path = "/posts/user/{uid}")
    public ResponseEntity<?> getPostByUserId(@PathVariable String uid) {
        List<Post> posts = postService.getPostsByUserId(uid);
        if(posts == null || posts.isEmpty()) {
            return new ResponseEntity<>(null, HttpStatus.NOT_FOUND);
        }

        return new ResponseEntity<>(posts, HttpStatus.OK); 
    }

    @PostMapping(path = "/posts")
    public ResponseEntity<?> savePost(@RequestBody Post post) {
        boolean isPostSaved = postService.savePost(post);
        if(isPostSaved) {
            return new ResponseEntity<>(HttpStatus.CREATED);
        }

        return new ResponseEntity<>(HttpStatus.BAD_REQUEST);
    }
}
