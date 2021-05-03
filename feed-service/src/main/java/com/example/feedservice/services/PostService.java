package com.example.feedservice.services;

import java.time.LocalDateTime;
import java.time.format.DateTimeFormatter;
import java.util.List;

import com.example.feedservice.cache.PostCache;
import com.example.feedservice.dataaccess.PostRepository;
import com.example.feedservice.models.Post;
import com.example.feedservice.interfaces.IPostService;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import lombok.extern.slf4j.Slf4j;

@Service
@Slf4j
public class PostService implements IPostService {
    private final PostRepository postRepository;
    private final PostCache postCache;

    @Autowired
    public PostService(PostRepository postRepository, PostCache postCache) {
        this.postRepository = postRepository;
        this.postCache = postCache;
    }

    @Override
    public boolean savePost(Post post) {
        try {
            post.setPostedOn(getCurrentDateTime());

            // Save to db
            postRepository.save(post);
            log.info("savePost - Post inserted to db");

            // cache post
            boolean postCached = postCache.savePost(post);
            log.info("savePost - Cached the post");

            return postCached;
        } catch (Exception e) {
            log.error("savePost - " + e.toString());
            return false;
        }
    }

    @Override
    public List<Post> getPostsByUid(String uid) {
        try {
            List<Post> posts = null;

            // get from cache
            log.info("getPostsByUid - Checking posts in cache");
            posts = postCache.getPostsByUid(uid);

            if (posts != null && !posts.isEmpty()) {
                return posts;
            }

            // get from db
            log.info("getPostsByUid - Getting posts from db");
            posts = postRepository.findPostsByUid(uid);

            // cache posts
            for (Post post : posts) {
                if(!postCache.savePost(post)) {
                    return null;
                }
            }

            return posts;
        } catch (Exception e) {
            log.error("getPostsByUid - " + e.toString());
            return null;
        }
    }
    
    @Override
    public List<Post> getAllPosts() {
        try {
            //get from db
            List<Post> posts = postRepository.findAll();
            return posts;
        } catch (Exception e) {
            log.error("getAllPosts - "+e.getMessage());
            return null;
        }
    }
    
    private String getCurrentDateTime() {
        LocalDateTime myDateObj = LocalDateTime.now();
        System.out.println("Before formatting: " + myDateObj);
        DateTimeFormatter myFormatObj = DateTimeFormatter.ofPattern("dd-MM-yyyy HH:mm:ss");
    
        String formattedDate = myDateObj.format(myFormatObj);
        return formattedDate;
    }
}
