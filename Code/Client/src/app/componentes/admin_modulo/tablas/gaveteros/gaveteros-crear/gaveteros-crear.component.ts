import { Component, EventEmitter, Input, Output, signal, WritableSignal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Gaveteros } from '../../../../../models/admin/Gaveteros';
import { GaveteroService } from '../../../../../services/APIS/Gavetero/gavetero.service';
import { MuebleService } from '../../../../../services/APIS/Mueble/mueble.service';

@Component({
  selector: 'app-gaveteros-crear',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './gaveteros-crear.component.html',
  styleUrl: './gaveteros-crear.component.css'
})
export class GaveterosCrearComponent {

  @Input() botoncrear: WritableSignal<boolean> = signal(true);
  @Output() Actualizar = new EventEmitter<void>();

  muebles : string[] = [];
  gavetero : Gaveteros = new Gaveteros();


  constructor(private gaveteroapi : GaveteroService , private mueblesAPI : MuebleService){}; 

  ngOnInit(){
    this.cargarMuebles();
  }

  cargarMuebles(){
    this.mueblesAPI.obtenerMuebles().subscribe({
      next: (data) => {
        this.muebles = data.map(mueble => mueble.Nombre);
      },
      error: (error) => {
        alert(error.error.error + ': ' + error.error.mensaje);
      }
    })
  }


  registrar(){

    this.gaveteroapi.crearGavetero(this.gavetero).subscribe(
      response => {
        this.Actualizar.emit(); 
        this.cerrar();
      },
      error => {
        alert(error.error.error + ': ' + error.error.mensaje);
      }
    );
   
  }

  cerrar(){
    this.botoncrear.set(false);
  }

}
