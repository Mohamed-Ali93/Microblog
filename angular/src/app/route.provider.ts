import { RoutesService, eLayoutType } from '@abp/ng.core';
import { APP_INITIALIZER } from '@angular/core';

export const APP_ROUTE_PROVIDER = [
  { provide: APP_INITIALIZER, useFactory: configureRoutes, deps: [RoutesService], multi: true },
];

function configureRoutes(routes: RoutesService) {
  return () => {
    routes.add([
      {
        path: '/',
        name: '::Menu:Home',
        iconClass: 'fas fa-home',
        order: 1,
        layout: eLayoutType.application,
      },
      // {
      //   path: '/books',
      //   name: '::Menu:Books',
      //   iconClass: 'fas fa-book',
      //   layout: eLayoutType.application,
      //   requiredPolicy: 'Microblog.Books',
      // },
      {
        path: '/posts',
        name: '::Menu:Posts',
        iconClass: 'fas fa-book',
        layout: eLayoutType.application,
      },
    ]);
  };
}
