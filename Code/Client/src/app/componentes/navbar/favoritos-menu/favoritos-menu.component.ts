import { Component, Input, WritableSignal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { FavoritosService } from '../../../services/favoritos/favoritos.service';
import { GrupoEquipo } from '../../../models/grupo_equipo';

@Component({
  selector: 'app-favoritos-menu',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './favoritos-menu.component.html',
  styleUrl: './favoritos-menu.component.css'
})
export class FavoritosMenuComponent {
  @Input() showFavoritos!: WritableSignal<boolean>;
  favoritos: GrupoEquipo[] = [];

  constructor(
    private favoritosService: FavoritosService,
  ) {}

  ngOnInit() {
    this.favoritosService.obtenerFavoritos().subscribe(favoritos => {
      this.favoritos = favoritos;
    });
  }
  cerrarMenu() {
    this.showFavoritos.set(false);
  }
}
