package com.example.feedservice.interfaces;

import java.util.List;

import com.example.feedservice.models.Post;

public interface IPostService {
	public List<Post> getAllPosts();
	public Post getPostById(int postId);
	public boolean savePost(Post post);
}
