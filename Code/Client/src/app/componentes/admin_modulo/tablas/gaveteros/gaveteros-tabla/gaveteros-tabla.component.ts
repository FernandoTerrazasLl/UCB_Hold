import { Component, OnInit, signal, WritableSignal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Gaveteros } from '../../../../../models/admin/Gaveteros';
import { GaveterosCrearComponent } from '../gaveteros-crear/gaveteros-crear.component';
import { GaveterosEditarComponent } from '../gaveteros-editar/gaveteros-editar.component';
import { GaveteroService } from '../../../../../services/APIS/Gavetero/gavetero.service';



@Component({
  selector: 'app-gaveteros-tabla',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule , GaveterosCrearComponent , GaveterosEditarComponent],
  templateUrl: './gaveteros-tabla.component.html',
  styleUrls: ['./gaveteros-tabla.component.css']
})
export class GaveterosTablaComponent {

  botoncrear : WritableSignal<boolean> = signal(false);
  botoneditar : WritableSignal<boolean> = signal(false);

  alertaeliminar : boolean = false;
  gaveteros : Gaveteros[] = [];
  gaveteroscopia: Gaveteros[] = [];

  gaveteroSeleccionado:  Gaveteros= new Gaveteros(); 

  terminoBusqueda: string = '';


  constructor(private gaveterosapi : GaveteroService){}; 




  ngOnInit(){
    this.cargarGaveteros();
  }


  limpiarGaveteroSeleccionado() {
    this.gaveteroSeleccionado = new Gaveteros();
  }



  creargarvetero() {
    this.botoncrear.set(true);
  }

  cargarGaveteros() {
 
    this.gaveterosapi.obtenerGaveteros().subscribe(
      (data: Gaveteros[]) => {
        this.gaveteros = data;
        this.gaveteroscopia = [...this.gaveteros]; 
      },
      (error) => {
        console.error('Error al cargar los gaveteros:', error);
      }
    );

  }

buscar(){
  if(this.terminoBusqueda.trim() === '') {
    this.limpiarBusqueda(); 
    return ; 
  }

  this.gaveteros = this.gaveteroscopia.filter(gavetero =>
    (gavetero.Nombre|| '').toLowerCase().includes(this.terminoBusqueda.toLowerCase()) ||
    (gavetero.Tipo ||'').toLowerCase().includes(this.terminoBusqueda.toLowerCase()) ||
    (gavetero.NombreMueble || '').toLowerCase().includes(this.terminoBusqueda.toLowerCase())
  );
}

limpiarBusqueda(){
  this.terminoBusqueda = '';
  this.gaveteros = [...this.gaveteroscopia]; 
  
}

editarGavetero(gavetero : Gaveteros) {
  this.botoncrear.set(false);
  this.gaveteroSeleccionado = { ...gavetero }; // Crear una copia del objeto
  this.botoneditar.set(true);
}

eliminarGavetero(gavetero : Gaveteros) {
  this.gaveteroSeleccionado = gavetero;
  this.alertaeliminar = true;
 
}

confirmarEliminacion() {
  this.gaveterosapi.eliminarGavetero(this.gaveteroSeleccionado.Id).subscribe(
    (response) => {

       this.cargarGaveteros(); 

    },
    (error) => {
      alert('Error al eliminar el gavetero: ' + error);
    }
  );
  this.limpiarGaveteroSeleccionado();
  this.alertaeliminar = false;
}

cancelarEliminacion(){
  this.alertaeliminar = false; 
  this.limpiarGaveteroSeleccionado(); 
}



}
