package psycho.euphoria.download

data class TaskState(var id: Long, var speed: Long,
                     var current: Long, var total: Long,
                     var title: String?)