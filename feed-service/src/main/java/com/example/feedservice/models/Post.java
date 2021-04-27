package com.example.feedservice.models;

import java.time.LocalDate;

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
public class Post {
	@Id
	@SequenceGenerator(
		name = "post_sequence",
		sequenceName = "post_sequence",
		allocationSize = 1
	)
	@GeneratedValue(
		strategy = GenerationType.SEQUENCE,
		generator = "post_sequence"
	)
	private int PostId;
	private String UId;
	private LocalDate PostedOn;
	private String Content;
}
