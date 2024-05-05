import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';

interface TreeItem {
  id: string;
  name: string;
  surName: string;
  birthDate: Date;
  identityNumber: string;
  children: TreeItem[];
  hasMoreChildren: boolean;
}

@Component({
  selector: 'app-tree-list',
  templateUrl: './tree-list.component.html',
  styleUrls: ['./tree-list.component.css'],
})
export class TreeListComponent implements OnInit {
  @Input() member: TreeItem | null = null;

  @Output() loadMoreEvent = new EventEmitter<string>();

  isExpanded = true;

  ngOnInit(): void {
    if (this.member?.hasMoreChildren) {
      this.isExpanded = false;
    }
  }

  toggleChildren() {
    if (this.member?.hasMoreChildren) {
      this.loadMoreEvent.emit(this.member.id);
      this.member.hasMoreChildren = false;
      this.isExpanded = true;
    } else {
      this.isExpanded = !this.isExpanded;
    }
  }
}
