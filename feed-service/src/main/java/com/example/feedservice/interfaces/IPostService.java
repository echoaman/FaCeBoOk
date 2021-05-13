package com.example.feedservice.interfaces;

import java.util.List;
import java.util.concurrent.CompletableFuture;

import com.example.feedservice.models.Post;
public interface IPostService {
    public CompletableFuture<List<Post>> getPostsByUid(String uid);
    public CompletableFuture<Boolean> savePost(Post post);
    public CompletableFuture<List<Post>> getAllPosts();
    public CompletableFuture<List<Post>> getFeedForUid(String uid);
}
