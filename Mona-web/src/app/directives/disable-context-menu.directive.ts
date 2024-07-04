import { Directive, HostListener } from '@angular/core';

@Directive({
  selector: '[appDisableContextMenu]'
})
export class DisableContextMenuDirective {

  @HostListener('contextmenu', ['$event'])
  onRightClick(event: MouseEvent) {
    event.preventDefault();
    console.log('Right-click menu disabled on this element');
  }
}

