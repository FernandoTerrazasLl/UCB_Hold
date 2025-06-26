import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { GrupoequipoService } from '../APIS/GrupoEquipo/grupoequipo.service';
import { GrupoEquipo } from '../../models/grupo_equipo';

@Injectable({
  providedIn: 'root'
})
export class FavoritosService {
  private favoritosSubject = new BehaviorSubject<GrupoEquipo[]>([]);
  public favoritos$ = this.favoritosSubject.asObservable();

  constructor(private grupoEquipoService: GrupoequipoService) {
    this.cargarFavoritos();
  }

  cargarFavoritos(): void {
    this.grupoEquipoService.obtenerFavoritos().subscribe(favoritos => {
      this.favoritosSubject.next(favoritos || []);
    });
  }

  obtenerFavoritos(): Observable<GrupoEquipo[]> {
    return this.favoritos$;
  }

  obtenerCantidadFavoritos(): number {
    return this.favoritosSubject.value.length;
  }

  actualizarFavoritos(): void {
    this.cargarFavoritos();
  }
}
