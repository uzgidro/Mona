import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewGroupInfoComponent } from './view-group-info.component';

describe('ViewGroupInfoComponent', () => {
  let component: ViewGroupInfoComponent;
  let fixture: ComponentFixture<ViewGroupInfoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ViewGroupInfoComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ViewGroupInfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
