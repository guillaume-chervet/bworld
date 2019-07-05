const deleteElement = function(elementChild, parent) {
  if (parent.childs) {
    const childs = parent.childs;
    while (childs.indexOf(elementChild) !== -1) {
      childs.splice(childs.indexOf(elementChild), 1);
    }
  }
};

const up = function(elementChild, parent) {
  const childs = parent.childs;
  const index = childs.indexOf(elementChild);
  childs.splice(index, 1);
  childs.splice(index - 1, 0, elementChild);
};

const down = function(elementChild, parent) {
  const childs = parent.childs;
  const index = childs.indexOf(elementChild);
  childs.splice(index, 1);
  childs.splice(index + 1, 0, elementChild);
};

const canUp = function(elementChild, parent) {
  if (parent.childs.indexOf(elementChild) <= 0) {
    return false;
  }
  return true;
};

const canDown = function(elementChild, parent) {
  if (parent.childs.indexOf(elementChild) >= parent.childs.length - 1) {
    return false;
  }
  return true;
};

export const menu = {
  up: up,
  down: down,
  canUp: canUp,
  canDown: canDown,
  deleteElement: deleteElement,
};
