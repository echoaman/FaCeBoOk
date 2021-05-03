package com.example.feedservice.interfaces;

import java.util.List;

import com.example.feedservice.models.Post;

public interface IPostCache {
    public boolean savePost(Post post);
    public List<Post> getPostsByUid(String uid);
}
