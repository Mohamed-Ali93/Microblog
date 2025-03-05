import type { IFormFile } from '../microsoft/asp-net-core/http/models';
import type { AuditedEntityDto, PagedAndSortedResultRequestDto } from '@abp/ng.core';

export interface CreatePostDto {
  content: string;
  image: File;
}

export interface GeoCoordinateDto {
  latitude: number;
  longitude: number;
}

export interface PostDto extends AuditedEntityDto<string> {
  content?: string;
  username?: string;
  originalImageUrl?: string;
  hasImage: boolean;
  isImageProcessed: boolean;
  location: GeoCoordinateDto;
  bestMatchImage: ProcessedImageDto;
}

export interface PostListDto extends PagedAndSortedResultRequestDto {
  screenWidth: number;
  screenHeight: number;
}

export interface ProcessedImageDto {
  id?: string;
  width: number;
  height: number;
  url?: string;
}
