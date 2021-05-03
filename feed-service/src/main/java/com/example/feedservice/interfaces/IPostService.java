package com.example.feedservice.interfaces;

import java.util.List;

import com.example.feedservice.models.Post;

public interface IPostService {
    public List<Post> getPostsByUid(String uid);
    public boolean savePost(Post post);
    public List<Post> getAllPosts();
}
