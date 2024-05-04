import { Component } from '@angular/core';

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
  styleUrl: './family-tree.component.css',
})
export class FamilyTreeComponent {
  identityNumber: string = '';
  familyTree: FamilyMember[] = [];
  loading = false;
  showAlert = false;
  errorMessage = '';
  maxLevels = 10;
  searchCompleted = false;

  // Mock family tree data
  mockData: FamilyMember[] = [
    {
      id: '1',
      name: 'Grand Parent',
      surName: '',
      birthDate: new Date(570, 3, 22),
      identityNumber: 'M001',
      hasMoreChildren: false,
      children: [
        {
          id: '2',
          name: 'Person 1',
          surName: 'PP',
          birthDate: new Date(605, 3, 1),
          identityNumber: 'F001',
          children: [
            {
              id: '3',
              name: 'Person 2',
              surName: 'PP',
              birthDate: new Date(605, 3, 1),
              identityNumber: 'H001',
              children: [],
              hasMoreChildren: false,
            },
            {
              id: '4',
              name: 'Person 3',
              surName: 'PP',
              birthDate: new Date(605, 3, 1),
              identityNumber: 'H002',
              children: [
                {
                  id: '5',
                  name: 'Person 4',
                  surName: 'PP',
                  birthDate: new Date(605, 3, 1),
                  identityNumber: 'Z004',
                  children: [
                    {
                      id: '5',
                      name: 'Person 5',
                      surName: 'PP',
                      birthDate: new Date(605, 3, 1),
                      identityNumber: 'Z005',
                      children: [
                        {
                          id: '6',
                          name: 'Person 6',
                          surName: 'PP',
                          birthDate: new Date(605, 3, 1),
                          identityNumber: 'Z006',
                          children: [
                            {
                              id: '7',
                              name: 'Person 7',
                              surName: 'PP',
                              birthDate: new Date(605, 3, 1),
                              identityNumber: 'Z007',
                              children: [
                                {
                                  id: '8',
                                  name: 'Person 8',
                                  surName: 'PP',
                                  birthDate: new Date(605, 3, 1),
                                  identityNumber: 'Z008',
                                  children: [
                                    {
                                      id: '9',
                                      name: 'Person 9',
                                      surName: 'PP',
                                      birthDate: new Date(605, 3, 1),
                                      identityNumber: 'Z009',
                                      children: [],
                                      hasMoreChildren: true,
                                    },
                                  ],
                                  hasMoreChildren: false,
                                },
                              ],
                              hasMoreChildren: false,
                            },
                          ],
                          hasMoreChildren: false,
                        },
                      ],
                      hasMoreChildren: false,
                    },
                  ],
                  hasMoreChildren: false,
                },
              ],
              hasMoreChildren: false,
            },
          ],
          hasMoreChildren: false,
        },
        // Add more mock family members here as needed
      ],
    },
    // Add more top-level ancestors here
  ];
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

    // Simulate an API call
    setTimeout(() => {
      this.familyTree = this.mockData.filter(
        (member) => member.identityNumber === this.identityNumber
      );
      this.loading = false;
      this.searchCompleted = true;
    }, 2000);
  }
  // Mock function to load more children for a node
  loadMore(nodeId: string) {
    // Simulate an API call
    setTimeout(() => {
      const node = this.findNodeById(this.familyTree, nodeId);
      if (node) {
        // Mock additional children data
        node.children.push({
          id: '3',
          name: 'New Child',
          surName: 'of Node ' + nodeId,
          birthDate: new Date(),
          identityNumber: 'NC001',
          children: [],
          hasMoreChildren: false,
        });
      }
    }, 2000);
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
