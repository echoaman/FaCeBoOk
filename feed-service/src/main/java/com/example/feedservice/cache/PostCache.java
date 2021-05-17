package com.example.feedservice.cache;

import java.util.List;
import java.time.Duration;

import com.example.feedservice.interfaces.IPostCache;
import com.example.feedservice.models.Post;

import org.springframework.data.redis.core.HashOperations;
import org.springframework.data.redis.core.RedisTemplate;
import org.springframework.stereotype.Repository;

import lombok.extern.slf4j.Slf4j;

@Repository
@Slf4j
public class PostCache implements IPostCache {

    private final HashOperations hashOperations;
    private final RedisTemplate<String, Object> redisTemplate;

    private final long TEN = 10;

    public PostCache(RedisTemplate<String, Object> redisTemplate) {
        this.redisTemplate = redisTemplate;
        this.hashOperations = this.redisTemplate.opsForHash();
    }

    @Override
    public boolean savePost(Post post) {
        try {
            String key = "uid:" + post.getUid();
            String hashKey = "post:" + post.getPostid();
            hashOperations.put(key, hashKey, post);
            return hashOperations.getOperations().expire(key, Duration.ofMinutes(TEN));
        } catch (Exception e) {
            log.error(e.getMessage());
        }

        return false;
    }

    @Override
    public List<Post> getPostsByUid(String uid) {
        try {
            String key = "uid:" + uid;
            List<Post> posts = hashOperations.values(key);
            return posts;
        } catch (Exception e) {
            log.error(e.getMessage());
        }

        return null;
    }

    @Override
    public boolean saveAllPosts(List<Post> posts) {
        try {
            for (Post post : posts) {
                if (!savePost(post)) {
                    return false;
                }
            }
            return true;
        } catch (Exception e) {
            log.error(e.getMessage());
        }

        return false;
    }
}
