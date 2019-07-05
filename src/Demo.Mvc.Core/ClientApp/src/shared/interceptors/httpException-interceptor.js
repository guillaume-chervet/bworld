import { toast } from '../services/toastr-factory';
import window from '../../window';
import q from '../../q';

export const responseError = function(response) {
  const statusCode = response.status;
  if (
    (statusCode >= 500 || statusCode === 404) &&
    response.config.url.indexOf(window.params.baseUrl) != -1
  ) {
    //exceptionService.setIsInError(true);
    toast.error(
      'Une erreur inattendue est survenue.</br>Veillez nous-escusez.</br>Contactez-nous si le problème persiste.',
      'Erreur serveur'
    );
  }

  return q.reject(response);
};
