import type { CreatePostDto, PostDto, PostListDto } from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class PostService {
  apiName = 'Default';


  create = (input: CreatePostDto, config?: Partial<Rest.Config>) => {
    const formData = new FormData();
    formData.append('content', input.content);
    formData.append('image', input.image);

    return this.restService.request<any, PostDto>({
      method: 'POST',
      url: '/api/app/post',
      body: formData,
    },
      { apiName: this.apiName, ...config });
  }


  getTimeline = (input: PostListDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<PostDto>>({
      method: 'GET',
      url: '/api/app/post/timeline',
      params: { screenWidth: input.screenWidth, screenHeight: input.screenHeight, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
      { apiName: this.apiName, ...config });

  constructor(private restService: RestService) { }
}
