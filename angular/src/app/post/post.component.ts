import { Component } from '@angular/core';
import { PostDto, PostListDto, PostService } from '@proxy/posts';

@Component({
  selector: 'app-post',
  templateUrl: './post.component.html',
  styleUrl: './post.component.scss'
})
export class PostComponent {
  posts: PostDto[] = [];
  totalCount = 0;
  loading = false;
  page = 0;
  pageSize = 10;

  constructor(private postService: PostService) {}

  ngOnInit() {
    this.loadPosts();
  }

  loadPosts() {
    this.loading = true;
     // Create the PostListDto object
     const postListDto: PostListDto = {
      skipCount: this.page * this.pageSize, // Calculate skipCount
      maxResultCount: this.pageSize,
      screenWidth: window.innerWidth,
      screenHeight: window.innerHeight
  };
    this.postService.getTimeline(
      postListDto
    ).subscribe({
      next: (response) => {
        this.posts = [...this.posts, ...response.items];
        this.totalCount = response.totalCount;
        this.loading = false;
      },
      error: (err) => {
        console.error('Failed to load posts', err);
        this.loading = false;
      }
    });
  }

  onScroll() {
    if (this.posts.length < this.totalCount) {
      this.page++;
      this.loadPosts();
    }
  }
}