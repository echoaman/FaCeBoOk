package com.example.feedservice.models;

import java.io.Serializable;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.GeneratedValue;
import javax.persistence.GenerationType;
import javax.persistence.Id;
import javax.persistence.SequenceGenerator;
import javax.persistence.Table;

import lombok.Data;

@Entity
@Table
@Data
public class Post implements Serializable {
    @Id
    @SequenceGenerator(name = "post_sequence", sequenceName = "post_sequence", allocationSize = 1)
    @GeneratedValue(strategy = GenerationType.SEQUENCE, generator = "post_sequence")
    private Integer postid;

    @Column(length = 30, name = "uid", nullable = false)
    private String uid;

    @Column(length = 30, name = "postedOn", nullable = false)
    private String postedOn;

    @Column(length = 250, name = "content", nullable = false)
    private String content;
}
