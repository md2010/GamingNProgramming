import { Component } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-map-info',
  templateUrl: './map-info.component.html',
  styleUrls: ['./map-info.component.css'],
  standalone: true
  
})
export class MapInfoComponent {

  
  id : string | null = null
  sub : any;

  constructor( private route: ActivatedRoute, private router: Router) {  }

  ngOnInit() {
    this.sub = this.route.paramMap.subscribe((params) => {
      console.log(params);
      this.id = params.get('id');
    });
  }

  play(id : string): void {
    this.router.navigate(['task-play', 1]);
  }

  onBack(id : string): void {
    this.router.navigate(['student-dashboard']);
  }

  ngOnDestroy() {
    if (this.sub) this.sub.unsubscribe();
  }
}
