import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { PostRoutingModule } from './post-routing.module';
import { PostComponent } from './post.component';
import { share } from 'rxjs';
import { SharedModule } from '../shared/shared.module';
import { CreatePostComponent } from './create-post/create-post.component';


@NgModule({
  declarations: [
    PostComponent,
    CreatePostComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    PostRoutingModule,
    
  ]
})
export class PostModule { }
