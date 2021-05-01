package com.example.feedservice.dataaccess;

import java.util.List;

import com.example.feedservice.models.Post;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.stereotype.Repository;

@Repository
public interface PostRepository extends JpaRepository<Post, Integer>{

    @Query(value = "SELECT * FROM post WHERE uid = ?1", nativeQuery = true)
    List<Post> findPostsByUid(String uid);
}
