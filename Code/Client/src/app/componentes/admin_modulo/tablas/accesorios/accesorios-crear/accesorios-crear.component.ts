import { Component, EventEmitter, Input, Output, signal, WritableSignal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Accesorio } from '../../../../../models/admin/Accesorio';
import { AccesoriosService } from '../../../../../services/APIS/Accesorio/accesorios.service';
import { EquipoService } from '../../../../../services/APIS/Equipo/equipo.service';
import { Equipos } from '../../../../../models/admin/Equipos';

@Component({
  selector: 'app-accesorios-crear',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './accesorios-crear.component.html',
  styleUrl: './accesorios-crear.component.css'
})
export class AccesoriosCrearComponent {

  @Input() botoncrear: WritableSignal<boolean> = signal(true);
  @Output() Actualizar = new EventEmitter<void>();

  equipos : Equipos[] = [] ;  

  accesorio : Accesorio = new Accesorio();


  constructor(private accesorioapi : AccesoriosService , private equipoAPI : EquipoService){}; 


  ngOnInit(){
    this.cargarEquipos();
  }

  cargarEquipos(){
    this.equipoAPI.obtenerEquipos().subscribe({
      next: (data) => {
        this.equipos = data;
      },
      error: (error) => {
        alert(error.error.error + ': ' + error.error.mensaje);
      }
    })
  }


 
  registrar(){

    this.accesorioapi.crearAccesorio(this.accesorio).subscribe(
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
