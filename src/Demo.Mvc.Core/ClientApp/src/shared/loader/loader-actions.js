import { LOADER_ADD, LOADER_CLEAR, LOADER_REMOVE } from './loader-types';

export const loaderAdd = message => {
  return {
    type: LOADER_ADD,
    message,
  };
};

export const loaderRemove = () => ({
  type: LOADER_REMOVE,
});

export const loaderClear = () => ({
  type: LOADER_CLEAR,
});

/*
export const fetchPlacesAsync = placesListServices => async (dispatch) => {
  dispatch(receivePlacesStart());
  const places = await placesListServices.loadPlaces();
  if (places) {
    dispatch(receivePlaces(places));
  } else {
    dispatch(receivePlacesError());
  }
};

export const filterText = value => ({ type: PLACES_FILTER_TEXT, value });

export const filterPaging = value => ({ type: PLACES_FILTER_PAGING, value });*/
