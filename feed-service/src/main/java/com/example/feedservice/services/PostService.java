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
            
            // save to cache
            boolean postCached = postCache.savePost(post);

            List<Post> posts = postRepository.findAll();
            
            //update cache
            boolean allPostsCached = postCache.saveAllPosts(posts);
            return postCached && allPostsCached;
        } catch (Exception e) {
            log.error(e.toString());
            return false;
        }
    }

    @Override
    public List<Post> getAllPosts() {
        try {
            List<Post> posts = null;
            posts = postCache.getAllPosts();
            
            //check cache
            if(posts != null && posts.size() > 0) {
                return posts;
            }
            
            // get from db
            posts = postRepository.findAll();
            
            // save in cache
            boolean isCached = postCache.saveAllPosts(posts);
            if(isCached){
                return posts;
            }

            return null;
            
        } catch (Exception e) {
            log.error(e.toString());
            return null;
        }
    }

    @Override
    public Post getPost(int postid) {
        try {
            Post post = null;
            
            //get from cache
            post = postCache.getPostById(postid);
            if(post != null) {
                return post;
            }

            //get from db
            post = postRepository.findById(postid).get();

            //save to cache
            boolean postCached = postCache.savePost(post);
            if(postCached) {
                return post;
            }

            return null;
        } catch (Exception e) {
            log.error(e.toString());
            return null;
        }
    }

    @Override
    public List<Post> getPostsByUserId(String uid) {
        try {
            return postRepository.findPostsByUid(uid);
        } catch (Exception e) {
            log.error(e.toString());
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
