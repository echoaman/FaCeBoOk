package com.example.feedservice.interfaces;

import java.util.List;
import java.util.concurrent.Future;

import com.example.feedservice.models.Post;
public interface IPostService {
    public Future<List<Post>> getPostsByUid(String uid) throws Exception;
    public Future<Boolean> savePost(Post post) throws Exception;
    public Future<List<Post>> getAllPosts() throws Exception;
    public List<Post> getFeedForUid(String uid) throws Exception;
}
