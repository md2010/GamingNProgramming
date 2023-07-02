import { Component } from '@angular/core';
import { AvatarModule } from '@coreui/angular';
import { Output, EventEmitter } from '@angular/core';
import { LookupService } from 'src/app/services/LookupService';
import { filter } from 'rxjs';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-select-avatar',
  templateUrl: './select-avatar.component.html',
  styleUrls: ['./select-avatar.component.css'],
  imports: [AvatarModule, CommonModule],
  standalone: true,
  providers: [LookupService]
})

export class SelectAvatarComponent {

  constructor(private lookupService: LookupService) { }
  private src = '';
  avatars = new Array<any>();
  womanAvatars = new Array<any>();
  manAvatars = new Array<any>();

  ngOnInit() {
    this.lookupService.getAvatars().subscribe(response => {
      this.avatars = response;
      this.filterAvatars();
  })
  }

  @Output() avatarSelectedEvent = new EventEmitter<string>();

  filterAvatars() {
    this.womanAvatars = this.avatars.filter(v => v.name.includes('Woman'));
    this.manAvatars = this.avatars.filter(v => v.name.includes('Man'));
  }

  onAvatarSelected(value: string) {
    this.src = value;
    this.avatarSelectedEvent.emit(value);
  }
}
