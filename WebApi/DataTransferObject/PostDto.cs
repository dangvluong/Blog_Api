﻿using WebApi.Models;

namespace WebApi.DataTransferObject
{
    public class PostDto
    {       
        public int Id { get; set; }        
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }       
        public DateTime DateCreated { get; set; }
       
        public DateTime? DateModifier { get; set; }
        public bool IsActive { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
        public Category Category { get; set; }      
        public MemberDto Author { get; set; }        
        public int CountView { get; set; }
    }
}