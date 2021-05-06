package com.example.feedservice.dataaccess;

import java.util.List;
import java.util.concurrent.CompletableFuture;

import com.example.feedservice.models.Post;

import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.CrudRepository;
import org.springframework.scheduling.annotation.Async;
import org.springframework.stereotype.Repository;

@Repository
public interface PostRepository extends CrudRepository<Post, Integer>{

    @Async
    @Query(value = "SELECT * FROM post WHERE uid = :uid", nativeQuery = true)
    public CompletableFuture<List<Post>> findPostsByUid(String uid);

    @Async
    @Query(value = "SELECT * FROM post", nativeQuery = true)
    public CompletableFuture<List<Post>> findAllPosts();

    @Async
    @Query(value = "INSERT INTO post (uid, postedOn, content) VALUES (:uid, :postedOn, :content)", nativeQuery = true)
    public CompletableFuture<Post> savePost(String uid, String postedOn, String content);
}
