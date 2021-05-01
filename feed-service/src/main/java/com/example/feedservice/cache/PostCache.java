package com.example.feedservice.cache;

import java.util.List;

import com.example.feedservice.interfaces.IPostCache;
import com.example.feedservice.models.Post;

import org.springframework.data.redis.core.HashOperations;
import org.springframework.data.redis.core.RedisTemplate;
import org.springframework.stereotype.Repository;

import lombok.extern.slf4j.Slf4j;

@Repository
@Slf4j
public class PostCache implements IPostCache {
    
    private HashOperations hashOperations;
    private RedisTemplate<String, Object> redisTemplate;

    private final String POST_HASH = "post";
    private final String ALL_POSTS_KEY = "all_posts";

    public PostCache(RedisTemplate<String, Object> redisTemplate){
        this.redisTemplate = redisTemplate;
        this.hashOperations = this.redisTemplate.opsForHash();
    }

    @Override
    public Post getPostById(int postid) {
        try {
            Post post = (Post)hashOperations.get(POST_HASH, postid);
            return post;
        } catch (Exception e) {
            log.error(e.toString());
            return null;
        }
    }

    @Override
    public List<Post> getAllPosts() {
        try {
            List<Post> posts = (List<Post>)hashOperations.get(POST_HASH, ALL_POSTS_KEY);
            return posts;
        } catch (Exception e) {
            log.error(e.toString());
            return null;
        }
    }

    @Override
    public boolean savePost(Post post) {
        try {
            hashOperations.put(POST_HASH, post.getPostid(), post);
            return true;
        } catch (Exception e) {
            log.error(e.toString());
            return false;
        }
    }

    @Override
    public boolean saveAllPosts(List<Post> posts) {
        try {
            hashOperations.put(POST_HASH, ALL_POSTS_KEY, posts);
            return true;
        } catch (Exception e) {
           log.error(e.toString());
           return false;
        }
    }
}
