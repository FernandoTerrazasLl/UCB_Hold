// navbar.component.ts
import { Component, Output, EventEmitter, signal, WritableSignal } from '@angular/core';
import { RouterModule } from '@angular/router';
import { CarritoService } from '../../services/carrito/carrito.service';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { UsuarioPrevioComponent } from './usuario-previo/usuario-previo.component';
import { NotificacionService } from '../../services/APIS/Notificacion/notificacion.service';
import { UsuarioService } from '../../services/usuario/usuario.service';
import { FavoritosService } from '../../services/favoritos/favoritos.service';
import { FavoritosMenuComponent } from './favoritos-menu/favoritos-menu.component';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [RouterModule, CommonModule, UsuarioPrevioComponent, FavoritosMenuComponent],
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css'],
})

export class NavbarComponent {
  showUserMenu: WritableSignal<boolean> = signal(false);
  showFavoritos: WritableSignal<boolean> = signal(false);
  notificaciones: boolean = false;
  cantidadFavoritos: number = 0;
  private intervalId: any;

  constructor(
    private carrito: CarritoService,
    private router: Router,
    private notificacionesAPI: NotificacionService,
    private usuario: UsuarioService,
    public favoritosService: FavoritosService
  ) { }

  ngOnInit() {
    this.notificacionesAPI.enviarnotificaciones();
    this.verificarnotificaciones();
    this.intervalId = setInterval(() => {
      this.notificacionesAPI.enviarnotificaciones();
      this.verificarnotificaciones();
    }, 1000);

    // Obtener cantidad de favoritos
    this.favoritosService.obtenerFavoritos().subscribe(favoritos => {
      this.cantidadFavoritos = favoritos.length;
    });
  }

  ngOnDestroy() {
    if (this.intervalId) {
      clearInterval(this.intervalId);
    }
  }

  botonhome() {
    this.router.navigate(['/home']);
  }

  botonnotificaciones() {
    this.router.navigate(['/Notificaciones']);
  }

  toggleUserMenu() {
    this.showUserMenu.set(!this.showUserMenu());
  }

  // Método simple para mostrar/ocultar favoritos
  toggleFavoritos() {
    this.showFavoritos.set(!this.showFavoritos());
  }

  totalproductos(): number {
    return this.carrito.obtenertotal();
  }

  mostrarcarrito() {
    this.router.navigate(['/Carrito']);
  }

  verificarnotificaciones() {
    if (this.usuario.vacio()) {
      this.notificaciones = false;
      return;
    }

    this.notificacionesAPI.verificarnoleidas(this.usuario.obtenercarnet()).subscribe({
      next: (data) => {
        this.notificaciones = data;
      },
      error: (error) => {
        console.error('Error verificando notificaciones:', error);
        this.notificaciones = false;
      }
    });
  }
}
