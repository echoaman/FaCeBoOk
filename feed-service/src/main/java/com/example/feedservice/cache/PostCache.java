package com.example.feedservice.cache;

import java.util.ArrayList;
import java.util.List;
import java.time.Duration;

import com.example.feedservice.interfaces.IPostCache;
import com.example.feedservice.models.Post;

import org.springframework.data.redis.connection.RedisConnection;
import org.springframework.data.redis.core.Cursor;
import org.springframework.data.redis.core.HashOperations;
import org.springframework.data.redis.core.RedisTemplate;
import org.springframework.data.redis.core.ScanOptions;
import org.springframework.scheduling.annotation.Async;
import org.springframework.stereotype.Repository;

import lombok.extern.slf4j.Slf4j;

@Repository
@Slf4j
public class PostCache implements IPostCache {

    private final HashOperations hashOperations;
    private final RedisTemplate<String, Object> redisTemplate;
    private final RedisConnection redisConnection;

    private final long TEN = 10;

    public PostCache(RedisTemplate<String, Object> redisTemplate) {
        this.redisTemplate = redisTemplate;
        this.hashOperations = this.redisTemplate.opsForHash();
        this.redisConnection = redisTemplate.getConnectionFactory().getConnection();
    }

    @Override
    @Async
    public boolean savePost(Post post) {
        try {
            String key = getKey(post.getUid(), post.getPostid().toString());
            hashOperations.put(key, "", post);
            return hashOperations.getOperations().expire(key, Duration.ofMinutes(TEN));
        } catch (Exception ex) {
            log.error("savePost - " + ex.getMessage());
            throw ex;
        }
    }

    @Override
    @Async
    public List<Post> getPostsByUid(String uid) {
        try {
            String pattern = getKey(uid, "*");
            ScanOptions scanOptions = ScanOptions.scanOptions().match(pattern).count(50).build();
            Cursor cursor = redisConnection.scan(scanOptions);
            List<Post> posts = new ArrayList<>();
            
            while(cursor.hasNext()) {
                String key = (String)cursor.next();
                posts.add((Post)hashOperations.get(key, ""));
            }

            return posts;
        } catch (Exception ex) {
            log.error("getPostsByUid - " + ex.getMessage());
            throw ex;
        }
    }

    private String getKey(String uid, String postid) {
        return "uid:" + uid + ":" + postid;
    }
    
}
