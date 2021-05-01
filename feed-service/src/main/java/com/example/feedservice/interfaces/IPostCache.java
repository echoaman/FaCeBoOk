package com.example.feedservice.interfaces;

import java.util.List;

import com.example.feedservice.models.Post;

public interface IPostCache {
    public Post getPostById(int postid);
    public List<Post> getAllPosts();
    public boolean savePost(Post post);
    public boolean saveAllPosts(List<Post> posts);
}
