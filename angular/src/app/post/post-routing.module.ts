import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PostComponent } from './post.component';
import { CreatePostComponent } from './create-post/create-post.component';

const routes: Routes = [{ path: '', component: PostComponent },{ path: 'create', component: CreatePostComponent }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PostRoutingModule { }
