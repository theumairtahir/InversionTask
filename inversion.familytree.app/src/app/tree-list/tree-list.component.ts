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
    // Determine if the current node should be expanded by default or not
    if (this.member?.hasMoreChildren) {
      this.isExpanded = false;
    }
  }

  // Toggle children display or emit an event if more children need to be loaded
  toggleChildren() {
    if (this.member?.hasMoreChildren) {
      // Emit event to load more children if the flag indicates so
      this.loadMoreEvent.emit(this.member.id);
      this.member.hasMoreChildren = false; // Assuming no further children will be loaded afterward
    } else {
      // Otherwise, toggle visibility
      this.isExpanded = !this.isExpanded;
    }
  }
}
