package com.example.feedservice;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.cache.annotation.EnableCaching;
import org.springframework.context.annotation.Bean;
import org.springframework.core.env.Environment;
import org.springframework.data.redis.connection.RedisPassword;
import org.springframework.data.redis.connection.RedisStandaloneConfiguration;
import org.springframework.data.redis.connection.lettuce.LettuceConnectionFactory;
import org.springframework.data.redis.core.RedisTemplate;
import org.springframework.data.redis.serializer.StringRedisSerializer;
import org.springframework.scheduling.annotation.EnableAsync;
import org.springframework.web.reactive.function.client.WebClient;

import lombok.extern.slf4j.Slf4j;

@SpringBootApplication
@EnableCaching
@EnableAsync
@Slf4j
public class FeedServiceApplication {

    @Value("${spring.redis.port}")
    private String REDIS_PORT;

    @Value("${spring.redis.host}")
    private String REDIS_HOST;

    @Value("${spring.redis.password}")
    private String REDIS_PWD;

    @Value("${spring.profiles.active}")
    private String REDIS_PROFILE;

    @Bean
    public LettuceConnectionFactory redisConnectionFactory() {
        System.out.println("-----------------------------------------------------------");
        log.info("redis host " + REDIS_HOST);
        log.info("redis port " + REDIS_PORT);
        log.info("redis password " + REDIS_PWD);
        log.info("redis profiles " + REDIS_PROFILE);
        System.out.println("-----------------------------------------------------------");
        RedisStandaloneConfiguration redisConf = new RedisStandaloneConfiguration();
        redisConf.setHostName(REDIS_HOST);
        redisConf.setPort(Integer.parseInt(REDIS_PORT));
        redisConf.setPassword(RedisPassword.of(REDIS_PWD));
        return new LettuceConnectionFactory(redisConf);
    }

    @Bean
    public RedisTemplate<String, Object> redisTemplate() {
        final RedisTemplate<String, Object> template = new RedisTemplate<String, Object>();
        template.setConnectionFactory(redisConnectionFactory());
        template.setHashKeySerializer(new StringRedisSerializer());
        template.setKeySerializer(new StringRedisSerializer());
        return template;
    }

    @Bean
    public WebClient.Builder getWebClientBuilder() {
        return WebClient.builder();
    }

    public static void main(String[] args) {
        SpringApplication.run(FeedServiceApplication.class, args);
    }

}