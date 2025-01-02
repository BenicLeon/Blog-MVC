
INSERT INTO users (username, password, email)
VALUES
('user1', '$2a$12$zOu7heN//Fiy2eup4L9vteT5CJUIIOSOTxNW8SYGJCHCVIaDDChtm', 'user1@example.com'),--lozinka za login (password1)
('user2', '$2a$12$xcuEUu3FTTbugKbKDTXSs.n/AYwtABmIQ/Wc6Ec2C02FqXe09f45O', 'user2@example.com');--lozinka za login (password1)


INSERT INTO blog_posts (title, content, created_at, updated_at, user_id)
VALUES
('Post 1 by user1', 'This is the content of the first post by user1.', GETDATE(), GETDATE(), 1),  
('Post 2 by user2', 'This is the content of the second post by user2.', GETDATE(), GETDATE(), 2);  

INSERT INTO comments (content, created_at, updated_at, user_id, blogpost_id)
VALUES
('This is a comment on post 1 by user1.', GETDATE(), GETDATE(), 1, 1),  
('This is a comment on post 2 by user2.', GETDATE(), GETDATE(), 2, 2);  
