package com.example.feedservice.controllers;

import java.util.List;

import com.example.feedservice.models.Post;
import com.example.feedservice.services.PostService;

import org.springframework.beans.factory.annotation.Autowired;
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
	public List<Post> getAllPosts() {
		return postService.getAllPosts();
	}

	@GetMapping(path = "/posts/{postId}")
	public Post getPostById(@PathVariable int postId) {
		return postService.getPostById(postId);
	}

	@PostMapping(path = "/posts")
	public boolean savePost(@RequestBody Post post) {
		return postService.savePost(post);
	}
}
