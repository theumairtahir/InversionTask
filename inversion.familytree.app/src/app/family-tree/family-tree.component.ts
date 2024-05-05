import { Component } from '@angular/core';
import { FamilyTreeService } from '../family-tree.service';

interface FamilyMember {
  id: string;
  name: string;
  surName: string;
  birthDate: Date;
  identityNumber: string;
  children: FamilyMember[];
  hasMoreChildren: boolean;
}

@Component({
  selector: 'app-family-tree',
  templateUrl: './family-tree.component.html',
  styleUrls: ['./family-tree.component.css'],
})
export class FamilyTreeComponent {
  identityNumber: string = '';
  familyTree: FamilyMember[] = [];
  loading = false;
  showAlert = false;
  errorMessage = '';
  searchCompleted = false;
  constructor(private familyTreeService: FamilyTreeService) {}

  onSearch() {
    this.searchCompleted = false;
    if (!this.identityNumber.match(/^[a-zA-Z0-9]+$/)) {
      this.showAlert = true;
      this.errorMessage = 'Please enter a valid alphanumeric Identity Number.';
      this.familyTree = [];
      return;
    }

    this.loading = true;
    this.showAlert = false;

    this.familyTreeService.getInitialData(this.identityNumber).subscribe({
      next: (data) => {
        this.familyTree = [];
        this.familyTree.push(data);
        this.loading = false;
        this.searchCompleted = true;
      },
      error: (err) => {
        this.showAlert = true;
        this.errorMessage = 'Error fetching data: ' + err.error;
        this.loading = false;
        this.searchCompleted = true;
      },
    });
  }

  loadMore(nodeId: string) {
    this.familyTreeService.loadMoreChildren(nodeId).subscribe({
      next: (newChildren) => {
        const node = this.findNodeById(this.familyTree, nodeId);
        if (node) {
          node.children.push(...newChildren);
        }
        this.loading = false;
      },
      error: (err) => {
        this.showAlert = true;
        this.errorMessage = 'Error fetching more data: ' + err.error;
        this.loading = false;
      },
    });
  }

  findNodeById(nodes: FamilyMember[], id: string): FamilyMember | null {
    for (const node of nodes) {
      if (node.id === id) return node;
      const found = this.findNodeById(node.children, id);
      if (found) return found;
    }
    return null;
  }
}
