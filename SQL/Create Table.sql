CREATE TABLE users (
    id INT IDENTITY(1,1) PRIMARY KEY,                
    username VARCHAR(255) NOT NULL,                   
    password VARCHAR(255) NOT NULL,                   
    email VARCHAR(255) NOT NULL                       
);
CREATE TABLE blog_posts (
    id INT IDENTITY(1,1) PRIMARY KEY,                
    title VARCHAR(255) NOT NULL,                     
    content TEXT NOT NULL,                            
    created_at DATETIME DEFAULT GETDATE(),           
    updated_at DATETIME DEFAULT GETDATE(),           
    user_id INT NOT NULL,                            
    FOREIGN KEY (user_id) REFERENCES users(id)       
);

CREATE TABLE comments (
    id INT IDENTITY(1,1) PRIMARY KEY,           
    content TEXT NOT NULL,                          
    created_at DATETIME DEFAULT GETDATE(),           
    updated_at DATETIME DEFAULT GETDATE(),           
    user_id INT NOT NULL,                            
    blogpost_id INT NOT NULL,                        
    FOREIGN KEY (user_id) REFERENCES users(id),      
    FOREIGN KEY (blogpost_id) REFERENCES blog_posts(id) 
);