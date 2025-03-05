import { Component } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { CreatePostDto, PostService } from '@proxy/posts';

@Component({
  selector: 'app-create-post',
  templateUrl: './create-post.component.html',
  styleUrl: './create-post.component.scss'
})
export class CreatePostComponent {
  postForm: FormGroup;
  selectedImage: File | null = null;
  previewUrl: string | ArrayBuffer | null = null;

  constructor(
    private fb: FormBuilder,
    private postService: PostService,
    private router: Router
  ) {
    this.postForm = this.fb.group({
      content: ['', [
        Validators.required, 
        Validators.maxLength(140)
      ]],
      image: [null]
    });
  }

  onFileSelected(event: any) {
    const file = event.target.files[0];
    if (file) {
      // Validate file type and size
      const allowedTypes = ['image/jpeg', 'image/png', 'image/webp'];
      const maxSize = 2 * 1024 * 1024; // 2MB

      if (!allowedTypes.includes(file.type)) {
        alert('Invalid file type. Only JPG, PNG, and WebP are allowed.');
        return;
      }

      if (file.size > maxSize) {
        alert('File size cannot exceed 2MB');
        return;
      }

      this.selectedImage = file;
      
      // Create preview
      const reader = new FileReader();
      reader.onload = () => {
        this.previewUrl = reader.result;
      };
      reader.readAsDataURL(file);
    }
  }

  removeImage() {
    this.selectedImage = null;
    this.previewUrl = null;
  }

  onSubmit() {
    if (this.postForm.valid) {
      console.log('Form submitted', this.postForm.value);
      const postData:CreatePostDto = {
        content: this.postForm.get('content')?.value,
        image: this.selectedImage
      };

      this.postService.create(postData).subscribe({
        next: () => {
          // Navigate back to timeline or show success message
          this.router.navigate(['/posts']);
        },
        error: (err) => {
          console.error('Failed to create post', err);
          // Handle error (show message to user)
        }
      });
    }
  }

}
