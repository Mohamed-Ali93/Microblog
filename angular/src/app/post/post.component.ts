import { Component } from '@angular/core';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { PostDto, PostListDto, PostService } from '@proxy/posts';
import { environment } from 'src/environments/environment';
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
  pageSize = 3;

  apiBaseUrl = `https://localhost:44367/api/app/post/image?blobName=`;
  itemsWithUrls: { imageUrl: string; content?: string; username?: string; originalImageUrl?: string; hasImage: boolean; isImageProcessed: boolean; location: import("d:/Work/Abjad/Microblog/angular/src/app/proxy/posts/models").GeoCoordinateDto; bestMatchImage: import("d:/Work/Abjad/Microblog/angular/src/app/proxy/posts/models").ProcessedImageDto; lastModificationTime?: string | Date; lastModifierId?: string; creationTime?: string | Date; creatorId?: string; id?: string; }[];

  constructor(private postService: PostService,
    private sanitizer: DomSanitizer

  ) {}

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
        this.itemsWithUrls = this.posts.map(item => ({
          ...item,
          imageUrl:  null,
        }));

        this.itemsWithUrls.forEach(post => {
          if (post.hasImage) {
            this.loadImageForPost(post);
          }
        });
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

  getImageUrl(blobName?: string): string {
    console.log(blobName);
    blobName = "3a1878a9-fc53-3114-6b0e-dbe5f82c2f01_1200x675.webp";
    return blobName ? `${this.apiBaseUrl}${blobName}` : 'assets/placeholder.png';
  }
  onImageError(event: Event) {
    console.log('Image error', event);
   // const imgElement = event.target as HTMLImageElement;
   // imgElement.src = 'assets/placeholder.png';
  }
  trackByFn(index: number, item: PostDto) {
    return item.id;
  }

  loadImageForPost(post: PostDto & { imageUrl?: SafeUrl }) {
    if (!post.id) return;

     // Check for the best fit image
  const imageIdToLoad = post.bestMatchImage?.url || post.originalImageUrl;
    console.log(imageIdToLoad);
    this.postService.getPostImage(imageIdToLoad).subscribe({
      next: (blob) => {
        const objectUrl = URL.createObjectURL(blob);
        post.imageUrl = this.sanitizer.bypassSecurityTrustUrl(objectUrl);
      },
      error: (err) => {
        console.error(`Failed to load image for post ${post.id}`, err);
      }
    });
  }
}