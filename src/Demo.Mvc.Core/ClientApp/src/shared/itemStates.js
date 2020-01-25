const itemStates = {
  published: 1,
  draft: 2,
  deleted: 3,
};

export const isDraft = menuItem => {
  return menuItem != null ? menuItem.state === itemStates.draft : false;
};
export const isDeleted = menuItem => {
  return menuItem != null ? menuItem.state === itemStates.deleted : false;
};

export const isPublished = menuItem => {
  return menuItem != null ? menuItem.state === itemStates.published : false;
};

export default itemStates;
