package com.example.feedservice.interfaces;

import java.util.List;

import com.example.feedservice.models.Post;

public interface IPostService {
    public List<Post> getAllPosts();
    public Post getPost(int postid);
    public List<Post> getPostsByUserId(String uid);
    public boolean savePost(Post post);
}
