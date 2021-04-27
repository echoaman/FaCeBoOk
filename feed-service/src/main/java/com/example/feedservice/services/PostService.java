package com.example.feedservice.services;

import java.util.List;

import com.example.feedservice.dataaccess.PostRepository;
import com.example.feedservice.models.Post;
import com.example.feedservice.interfaces.IPostService;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import lombok.extern.slf4j.Slf4j;

@Service
@Slf4j
public class PostService implements IPostService{
	private final PostRepository postRepository;

	@Autowired
	public PostService(PostRepository postRepository) {
		this.postRepository = postRepository;
	}

	public boolean savePost(Post post) {
		try {
			postRepository.save(post);
			return true;
		} catch (Exception e) {
			log.error(e.toString());
			return false;
		}
	}

	public List<Post> getAllPosts() {
		try {
			return postRepository.findAll();
		} catch (Exception e) {
			log.error(e.toString());
			return null;
		}
	}

	@Override
	public Post getPostById(int postId) {
		try {
			return postRepository.findById(postId).get();
		} catch (Exception e) {
			log.error(e.toString());
			return null;
		}
	}
}
